#!/bin/bash
docker-compose up -d

seconds=15
echo "waiting $seconds seconds until the containers are ready"
sleep $seconds

postman_dir="docs/api/postman"
newman run "$postman_dir/API Tests.postman_collection.json" -e "$postman_dir/Test env.postman_environment.json"

docker-compose down
