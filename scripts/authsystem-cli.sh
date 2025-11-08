#!/usr/bin/env bash
set -euo pipefail

HOST="${HOST:-https://localhost:5001}"
TENANT="${TENANT:-demo}"
TOKEN="${TOKEN:-}"

usage() {
  cat <<USAGE
Usage: $0 <command> [options]

Commands:
  login --email <email> --password <password>
  audit-rest --page <page> [--tenant <tenant>]
  audit-graphql --fields "field1 field2" [--tenant <tenant>]
  webhook-test --url <endpoint>

Environment Variables:
  HOST   Base URL (default: $HOST)
  TENANT Tenant identifier (default: $TENANT)
  TOKEN  Bearer token reused by audit commands when provided
USAGE
}

command=${1:-}
shift || true

case "$command" in
  login)
    EMAIL=""
    PASSWORD=""
    while [[ $# -gt 0 ]]; do
      case "$1" in
        --email) EMAIL="$2"; shift 2 ;;
        --password) PASSWORD="$2"; shift 2 ;;
        *) echo "Unknown option $1"; exit 1 ;;
      esac
    done
    curl -sS -X POST "$HOST/api/v1/auth/login" \
      -H "Content-Type: application/json" \
      -H "X-Tenant-Id: $TENANT" \
      -d "{\"email\":\"$EMAIL\",\"password\":\"$PASSWORD\"}"
    ;;
  audit-rest)
    PAGE=1
    while [[ $# -gt 0 ]]; do
      case "$1" in
        --page) PAGE="$2"; shift 2 ;;
        --tenant) TENANT="$2"; shift 2 ;;
        *) echo "Unknown option $1"; exit 1 ;;
      esac
    done
    curl -sS "$HOST/api/v1/audit/security-events?page=$PAGE&pageSize=50" \
      -H "Authorization: Bearer ${TOKEN}" \
      -H "X-Tenant-Id: $TENANT"
    ;;
  audit-graphql)
    FIELDS="id eventType occurredAtUtc"
    while [[ $# -gt 0 ]]; do
      case "$1" in
        --fields) FIELDS="$2"; shift 2 ;;
        --tenant) TENANT="$2"; shift 2 ;;
        *) echo "Unknown option $1"; exit 1 ;;
      esac
    done
    GRAPHQL_QUERY="{ securityEvents(page:1, tenantId: \"$TENANT\") { items { $FIELDS } } }"
    curl -sS -X POST "$HOST/graphql" \
      -H "Content-Type: application/json" \
      -H "Authorization: Bearer ${TOKEN}" \
      -d "{\"query\":\"$GRAPHQL_QUERY\"}"
    ;;
  webhook-test)
    URL=""
    while [[ $# -gt 0 ]]; do
      case "$1" in
        --url) URL="$2"; shift 2 ;;
        *) echo "Unknown option $1"; exit 1 ;;
      esac
    done
    curl -sS -X POST "$URL" \
      -H "Content-Type: application/json" \
      -d '{"ping":"security-event"}'
    ;;
  ""|-h|--help)
    usage
    ;;
  *)
    echo "Unknown command: $command"
    usage
    exit 1
    ;;
 esac