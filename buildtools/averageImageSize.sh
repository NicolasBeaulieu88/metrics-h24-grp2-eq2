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

# Query the database to retrieve image sizes between the specified dates
query="SELECT image_size FROM build_logs WHERE start_time >= '$date1' AND start_time <= '$date2'"
image_sizes=$(psql -h 157.230.69.113 -U user02eq2 -d db02eq2 -t -c "$query")

# Calculate average image size
total_image_size=0
image_count=0
for size in $image_sizes; do
    total_image_size=$((total_image_size + size))
    image_count=$((image_count + 1))
done

if [[ $image_count -gt 0 ]]; then
    average_image_size=$((total_image_size / image_count))
    echo "La taille moyenne de l'image entre $date1 et $date2 est de $average_image_size octets."
else
    echo "Aucune donnée de construction trouvée entre $date1 et $date2."
fi
