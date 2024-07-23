#!/bin/bash
# usage: source curl.sh get|post|put|delete

method=$1

if ! [[ $method ]]; then
    echo "No HTTP method specified"
    exit 1
fi

case "${method^^}" in
  GET)
    curl http://localhost:5275/albums/ | jq .
    ;;

  POST)
    read -r -d '' json << END
    {
      "band": "Devourment",
      "name": "Molesting the Decapitated",
      "genre": "Slam Death Metal",
      "releaseDate": "1999-06-26"
    }
END
    curl \
      -d "$json" \
      -H "Content-Type: application/json" \
      -v http://localhost:5275/albums
    ;;

  PUT)
    read -r -d '' json << END
    {
      "band": "Devourment",
      "name": "Molesting the Decapitated",
      "genre": "Brutal/Slam Death Metal",
      "releaseDate": "1999-06-26"
    }
END
    curl \
      -d "$json" \
      -H "Content-Type: application/json" \
      -X PUT \
      -v http://localhost:5275/albums/4
    ;;

  DELETE)
    curl -v -X DELETE http://localhost:5275/albums/4
    ;;

  *)
    echo "Unknown HTTP method"
    exit 1
    ;;
esac
