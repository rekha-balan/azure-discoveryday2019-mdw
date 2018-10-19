mkdir moderndw
cd moderndw

wget https://raw.githubusercontent.com/plzm/didays2019-moderndw/master/SampleData-CSV/orders.csv
wget https://raw.githubusercontent.com/plzm/didays2019-moderndw/master/SampleData-CSV/orderitems.csv
wget https://raw.githubusercontent.com/plzm/didays2019-moderndw/master/SampleData-SQL/ModernDW.bacpac

azcopy --source ./ --destination https://[YOURSTORAGEACCT].blob.core.windows.net/[YOURCONTAINER]/ --dest-key [YOURKEY] --verbose