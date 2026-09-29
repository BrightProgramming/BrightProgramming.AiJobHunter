# PostgreSQL repository integration tests

Repository integration tests go in this folder. They run against a disposable PostgreSQL instance so they can run locally and in CI without relying on a developer's database.

The shared fixture starts a PostgreSQL container with Testcontainers and applies the API migrations before the repository tests run.
