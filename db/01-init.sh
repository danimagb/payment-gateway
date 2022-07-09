#!/bin/bash
set -e
export PGPASSWORD=$POSTGRES_PASSWORD;
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
  CREATE USER $APP_DB_USER WITH PASSWORD '$APP_DB_PASS';
  CREATE DATABASE $APP_DB_NAME;
  GRANT ALL PRIVILEGES ON DATABASE $APP_DB_NAME TO $APP_DB_USER;
  \connect $APP_DB_NAME $APP_DB_USER
  BEGIN;
    CREATE TABLE IF NOT EXISTS payments (
	  id uuid PRIMARY KEY,
    request_id uuid,
	  merchant_id uuid NOT NULL,
	  card_number text NOT NULL,
    card_cvv text NOT NULL,
    card_expiry_month integer NOT NULL,
    card_expiry_year integer NOT NULL,
	  card_holder_name text NOT NULL,
    amount decimal NOT NULL CHECK (amount > 0),
    currency text NOT NULL,
    created_at timestamptz NOT NULL,
    processed_at timestamptz,
    status text NOT NULL,
	  UNIQUE(request_id)
	);
	CREATE INDEX idx_merchant_id_request_id ON payments (merchant_id, request_id);
  COMMIT;
EOSQL