.PHONY: build infra run unit-tests integration-tests clean

build:
	@docker-compose up --build --no-start

infra:
	@docker-compose up -d payment-gateway-database

run: build infra
	@docker-compose up -d payment-gateway-api

unit-tests:
	@docker-compose up --build --no-start unit-tests
	@docker-compose up --abort-on-container-exit --exit-code-from unit-tests unit-tests

integration-tests: infra
	@docker-compose up --build --no-start integration-tests
	@docker-compose up --abort-on-container-exit --exit-code-from integration-tests integration-tests

database: clean infra

clean:
	@docker-compose down
