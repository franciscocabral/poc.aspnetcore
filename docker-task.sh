#!/bin/sh

NAME="risco-poc-aspnetcore"
TAG="dev"
ENV="debug"
PORT=50000
FILE="./Dockerfile"
DIR="."

CleanUp () {
  echo "Stoping container referents to $1"
  docker ps -a | grep "$1" | awk '{print $1}' | xargs docker stop

  echo "Deleting all non used Images, Containers, Volumes, and Networks"
  # Purging All Unused or Dangling Images, Containers, Volumes, and Networks
  docker system prune -f

  # Remove Containers
  echo "Removing container referents to $1"
  docker ps -a | grep "$1"| awk '{print $1}' | xargs docker rm

  # Remove Images
  echo "Removing images referents to $1"
  docker images -a | grep  "$1" | awk '{print $3}' | xargs docker rmi
}

Build () {
  docker build --force-rm -t "$1":"$2" --build-arg configuration="$3" --file "$4" "$5"
}

Run () {
  docker run -d --cpus=0.5 -m 300M -p "$3":80 "$1":"$2"
}

# Shows the usage for the script.
ShowUsage () {
  echo "Usage: dockerTask.sh [COMMAND] [OPTIONS] "
  echo "    Runs command"
  echo ""
  echo "Commands:"
  echo "    clean:        Removes the image '$NAME' and kills all containers based on this image."
  echo "         -n       Name, defalt '$NAME'"
  echo ""
  echo "    run:          Builds the debug image and runs docker container."
  echo "         -d       Execution directory, default '$DIR'"
  echo "         -e       Environment, default '$ENV'"
  echo "         -f       Docker file, default '$FILE'"
  echo "         -n       Name, defalt '$NAME'"
  echo "         -t       Tag, defalt '$TAG'"
  echo "         -p       Port, defalt '$PORT'"
  echo ""
  echo ""
  echo "Example:"
  echo "    ./docker-task.sh run -e $ENV -p $PORT"
  echo ""
  echo "    This will:"
  echo "        Build a Docker image named '$NAME' using debug environment and start a new Docker container."
}

Execute() {
  case "$1" in
    "clean")
      CleanUp $NAME
      ;;
    "run") 
      Build $NAME $TAG $ENV $FILE $DIR
      Run $NAME $TAG $PORT
      ;;
    *)
      ShowUsage
      ;;
  esac
}

while getopts ":d:e:f:n:p:t:" opt ${@:2}; do
  case "$opt" in
    d) DIR=$OPTARG ;;
    e) ENV=$OPTARG ;;
    f) FILE=$OPTARG ;;
    n) NAME=$OPTARG ;;
    p) PORT=$OPTARG ;;
    t) TAG=$OPTARG ;;
    \?) 
      echo "Invalid option -$OPTARG" >&2
      exit 1
      ;;
  esac
done

if [ $# -eq 0 ]; then
  ShowUsage
else
  Execute "$1"
fi