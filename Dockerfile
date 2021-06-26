#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
ARG source
COPY obj/Docker/publish /bin
WORKDIR /bin
ENTRYPOINT ["dotnet", "ProductManagement.dll"]
#COPY ${source:-obj/Docker/publish} .