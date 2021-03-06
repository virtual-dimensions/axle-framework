paket='../../../.paket/paket.sh'
project='Axle.Resources.Java.Tests'
project_format='csproj'

rm -rf paket-files/ && $paket update && rm -rf obj/ && mkdir obj/ && dotnet restore $project.$project_format
if [ $? -ne 0 ]; then
  read -rsp "Press [Enter] to quit"
  echo ""
  exit
fi