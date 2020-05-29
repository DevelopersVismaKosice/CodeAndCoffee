## Steps

1) Make src folder
2) Make src/VismaDocker.Web folder
3) run 'dotnet new webapp' in that folder
4) cd ..
5) create new solution file running 'dotnet new sln'
6) add project to solution using 'dotnet sln add VismaDocker.Web'
7) Create and adjust Dockerfile
8) Create and adjust .dockerignore
9) Now build the docker image using 'docker build --no-cache -t michal/visma.docker .'
   --no-cache Do not use cache when building the image
   -t Name and optionally a tag in the ‘name:tag’ format
10) Run docker image 'docker run --rm -it -p 8080:5000 michal/visma.docker'
   --rm Automatically remove the container when it exits
   -i  interactive
   -t  Allocate a pseudo-TTY
   -p  Publish a container’s port(s) to the host

## Dockerfile reference
https://docs.docker.com/engine/reference/builder/#workdir

1) The FROM instruction initializes a new build stage and sets the Base Image for subsequent instructions. As such, a valid Dockerfile must start with a FROM instruction. 
2) The WORKDIR instruction sets the working directory
3) The EXPOSE instruction informs Docker that the container listens on the specified network ports at runtime
4) The RUN instruction will execute any commands in a new layer on top of the current image and commit the results. The resulting committed image will be used for the next step in the Dockerfile.
5) The ENV instruction sets the environment variable
6) An ENTRYPOINT allows you to configure a container that will run as an executable.
Only the last ENTRYPOINT instruction in the Dockerfile will have an effect.

## Resources

1) Docker resources for .NET Core https://hub.docker.com/_/microsoft-dotnet-core/
2) https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md
3) https://code-maze.com/aspnetcore-app-dockerfiles/
