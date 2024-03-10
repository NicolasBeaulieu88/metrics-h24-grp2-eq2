#!/bin/bash

# Function to convert French date to YYYY-MM-DD format
convert_date() {
    french_date=$1
    # Split the date into day, month, and year
    day=$(echo "$french_date" | awk '{print $1}')
    month=$(echo "$french_date" | awk '{print $2}')
    year=$(echo "$french_date" | awk '{print $3}')

    case "$month" in
        janvier) month="01";;
        février) month="02";;
        mars) month="03";;
        avril) month="04";;
        mai) month="05";;
        juin) month="06";;
        juillet) month="07";;
        août) month="08";;
        septembre) month="09";;
        octobre) month="10";;
        novembre) month="11";;
        décembre) month="12";;
        *) echo "Mois invalide"; exit 1;;
    esac
    echo "$year-$month-$day"
}

# Prompt user to enter two dates in French
echo "Veuillez entrer la première date (format: jour mois année, par exemple: 1 janvier 2024):"
read date1
echo "Veuillez entrer la deuxième date (format: jour mois année, par exemple: 1 janvier 2024):"
read date2

# Convert French dates to YYYY-MM-DD format
date1=$(convert_date "$date1")
date2=$(convert_date "$date2")

# Query the database to retrieve build times between the specified dates
build_times=$(psql -h 157.230.69.113 -U user02eq2 -d db02eq2 -t -c "SELECT duration FROM build_logs WHERE start_time >= '$date1' AND start_time <= '$date2'")

# Calculate average build time
total_build_time=0
build_count=0
for build_time in $build_times; do
    total_build_time=$((total_build_time + build_time))
    build_count=$((build_count + 1))
done

if [[ $build_count -gt 0 ]]; then
    average_build_time=$((total_build_time / build_count))
    echo "Le temps de construction moyen entre $date1 et $date2 est de $average_build_time secondes."
else
    echo "Aucune donnée de construction trouvée entre $date1 et $date2."
fi
