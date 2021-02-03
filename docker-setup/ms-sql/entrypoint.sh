#!/bin/bash

# start SQL Server
/opt/mssql/bin/sqlservr &

password=Password123

# wait for MSSQL server to start
export STATUS=1
while [[ $STATUS -ne 0 ]] && [[ $i -lt 30 ]]; do
	i=$i+1
	/opt/mssql-tools/bin/sqlcmd -S 0.0.0.0,1433 -t 1 -U SA -P $password -Q "select 1" >> /dev/null
	STATUS=$?
done

if [ $STATUS -ne 0 ]; then
	echo "Error: MSSQL SERVER took more than thirty seconds to start up."
	exit 1
fi

echo "======= MSSQL CONFIG COMPLETE =======" | tee -a ./config.log

# Call extra command
eval $1