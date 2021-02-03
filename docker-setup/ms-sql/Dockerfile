FROM mcr.microsoft.com/mssql/server:2017-latest
RUN apt-get update && apt-get install -y dos2unix

# Create app directory
RUN mkdir -p /docker-setup/ms-sql/usr/src/app
WORKDIR /docker-setup/ms-sql/usr/src/app

# Bundle app source
COPY ./docker-setup/ms-sql/ /docker-setup/ms-sql/usr/src/app
ADD ./docker-setup/ms-sql/entrypoint.sh /docker-setup/ms-sql/usr/src/app

# Grant permissions for the import-data script to be executable
RUN dos2unix /docker-setup/ms-sql/usr/src/app/entrypoint.sh
RUN chmod +x /docker-setup/ms-sql/usr/src/app/entrypoint.sh

EXPOSE 1433
ENTRYPOINT ["/docker-setup/ms-sql/usr/src/app/entrypoint.sh"]

# Tail the setup logs to trap the process
CMD ["tail -f /dev/null"]