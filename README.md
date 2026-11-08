# Masroof

A multi-tenant expense platform for small businesses, built step by step as a learning project.

> Status: **in progress** (Phase 1: ASP.NET Core API + MongoDB)

## Planned architecture

```
Angular UI ──► Go LLM Gateway ──► LLM providers (Gemini / Groq, with fallback)
                    │
Angular UI ──► ASP.NET Core API ──► MongoDB (+ vector search)
                    │
                 (queue)
                    ▼
             Go Receipt Worker
```

## Roadmap

- [x] ASP.NET Core Web API project (.NET 10, controllers)
- [x] MongoDB as a single-node replica set in Docker (needed for transactions)
- [x] Secrets kept in .NET User Secrets, outside the repo
- [ ] Phase 1: MongoDB connection, tenants, JWT auth, expenses CRUD, reports with aggregation
- [ ] Phase 2: LLM receipt extraction (structured output) and "Ask AI" with tool calling
- [ ] Phase 3: RAG over company expense policies with citations and an eval set
- [ ] Phase 4: Go LLM gateway and Go receipt worker over a queue (outbox, idempotency)
- [ ] Phase 5: docker-compose, OpenTelemetry, production hardening
- [ ] Phase 6: Angular frontend

## Run locally

Requirements: .NET 10 SDK and Docker.

```bash
# MongoDB as a single-node replica set
docker run -d --name masroof_mongo -p 27017:27017 -v masroof_mongo_data:/data/db mongo:8 --replSet rs0
docker exec -it masroof_mongo mongosh --eval "rs.initiate({_id:'rs0',members:[{_id:0,host:'localhost:27017'}]})"

# Secrets (stored outside the repo)
cd Masroof.Api
dotnet user-secrets set "Mongo:ConnectionString" "mongodb://localhost:27017/?replicaSet=rs0"
dotnet user-secrets set "Mongo:Database" "masroof"

dotnet run
```
