# Docker in practice - Docker compose

Session Date: June 5 2020

- Docker compose is included in Docker for Windows / Mac. On Linux you can download binary from Compose repository on GitHub.
- :warning: BEWARE OF DOCKER COMPOSE FILE INDENTATION
- Docker compose version is tied to support from Docker engine, so check your Docker engine in order to determine Docker compose file format.
- Docker compose overriding:
   a) By default, Compose reads two files, a docker-compose.yml and an optional docker-compose.override.yml file
   b) By convention, the docker-compose.yml contains your base configuration
   c) To use multiple override files, or an override file with a different name, you can use the -f option to specify the list of files
   `docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d`
- Compose supports declaring default environment variables in an environment file named `.env` placed in the folder where the docker-compose command is executed
- The environment variables you define here are used for variable substitution in your Compose file, and can also be used to define the following CLI variables:

  - COMPOSE_API_VERSION
  - COMPOSE_CONVERT_WINDOWS_PATHS
  - COMPOSE_FILE
  - COMPOSE_HTTP_TIMEOUT
  - COMPOSE_TLS_VERSION
  - COMPOSE_PROJECT_NAME
  - DOCKER_CERT_PATH
  - DOCKER_HOST
  - DOCKER_TLS_VERIFY

## Important docker-compose commands

   1) `docker-compose build` Build or rebuild services
   2) `docker-compose config` Validate and view the Compose file
   3) `docker-compose up` Create and start containers
   4) `docker-compose ps` List containers
   5) `docker-compose down` Stop and remove containers, networks, images, and volumes
   6) other interesting commands include: log, start, stop, events, exec

## Networking

Docker’s networking subsystem is pluggable, using drivers. Several drivers exist by default, and provide core networking functionality:

- `bridge` - The default network driver. Standalone containers that need to communicate.
- `host` - Remove network isolation between the container and the Docker host, and use the host’s networking directly. Only available for swarm services on Docker 17.06 and higher
- `overlay` - Overlay networks connect multiple Docker daemons together and enable swarm services to communicate with each other.
- `macvlan` - Macvlan networks allow you to assign a MAC address to a container, making it appear as a physical device on your network. For legacy apps.
- `none` - Disable all networking.
- You can install and use third-party network plugins with Docker. These plugins are available from Docker Hub or from third-party vendors

## Session Steps

1) Run `dotnet new worker -n VismaDocker.ScraperWorker` to create scraper project
2) Add package AngleSharp to the Scraper solution using `dotnet add package AngleSharp` inside its folder
3) Add Dockerfile from VismaDocker.Web solution
4) Run 'dotnet sln add VismaDocker.ScraperWorker' in the location with solution file to add project to the solution
5) Build Dockerfile using command `docker build -t visma-docker.scraper-worker .`
6) Run `docker-compose up`

## Docker compose reference

<https://docs.docker.com/compose/>

Compose is a tool for defining and running multi-container Docker applications.

1) The FROM instruction initializes a new build stage and sets the Base Image for subsequent instructions. As such, a valid Dockerfile must start with a FROM instruction.
2) The WORKDIR instruction sets the working directory
3) The EXPOSE instruction informs Docker that the container listens on the specified network ports at runtime
4) The RUN instruction will execute any commands in a new layer on top of the current image and commit the results. The resulting committed image will be used for the next step in the Dockerfile.
5) The ENV instruction sets the environment variable
6) An ENTRYPOINT allows you to configure a container that will run as an executable.
Only the last ENTRYPOINT instruction in the Dockerfile will have an effect.

## :warning: Lessons from last session

Check your dockerfile for syntax errors - not being able to run the website inside container failed on one S letter: ENV ASPNETCORE_URLS="http://0.0.0.0:5000" versus ENV ASPNETCORE_URL="http://0.0.0.0:5000", URLS versus URL.

## Resources

1) <https://docs.docker.com/compose/>
2) <https://docs.docker.com/compose/compose-file/>
3) <https://docs.docker.com/compose/reference/overview/>
4) <https://medium.com/@vekzdran/practical-net-core-write-a-web-scraper-downloader-excel-parser-part-1-4-ece43e0af898>