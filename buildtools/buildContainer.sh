#!/bin/bash

# Start time of the build
start_time=$(date +"%s")

# Run Docker build
cd ../MetricsAPI-LOG680 || exit
docker build -t metricsapi .
cd ../buildtools || exit

# End time of the build
end_time=$(date +"%s")

# Calculate duration
duration=$((end_time - start_time))

# Get the size of the built image
image_size=$(docker image inspect --format='{{.Size}}' metricsapi)

# Insert build information into the PostgreSQL database
PGPASSWORD="Dw2OtjzSOKoZvrGN" psql -h 157.230.69.113 -U user02eq2 -d db02eq2 <<EOF
INSERT INTO build_logs (start_time, end_time, duration, image_size)
VALUES (to_timestamp($start_time), to_timestamp($end_time), $duration, $image_size);
EOF
