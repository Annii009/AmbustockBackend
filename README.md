docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=P@ssw0rd2025!" -p 8308:1433 --platform linux/amd64 -d mcr.microsoft.com/mssql/server:2022-latest
