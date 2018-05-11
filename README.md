# Proof of Concept - Aspnet Core

This is a proof of concept of using Aspnet Core 2.0 with multi stage Dockerfile to build, publish and run.

## Getting Started

To run this project, follow this steps:

```bash
# 1) Clone the project
git clone git@github.com:franciscocabral/poc.aspnetcore.git

# 2) Run the docker task
cd poc.aspnetcore
./docker-task.sh run

# 3) Test the result
curl --request GET \
  --url http://localhost:50000/api/profile/franciscocabral \
  --header 'Accept-Language: pt-BR' 
```

You can also build the project using [dotnet cli](https://docs.microsoft.com/pt-br/dotnet/core/tools/?tabs=netcore2x), or [Visual Studio](https://www.visualstudio.com/pt-br/downloads/?rr=https%3A%2F%2Fwww.google.com.br%2F).


## Prerequisites

You will need the [Docker CE](https://www.docker.com/community-edition#/download) to execute this tutorial.

## Running the tests

[TODO] In future verions I'll add the stage to run tests.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

## Authors

* **Francisco Cabral** - *Initial work* - [franciscocabral](https://github.com/franciscocabral)

See also the list of [contributors](https://github.com/franciscocabral/poc.aspnetcore/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details