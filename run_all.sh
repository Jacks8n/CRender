#!/usr/bin/env bash

ver=$(echo "$(lsb_release -r)" | grep -oP '\d*\.\d*')

#Register Microsoft key
wget https://packages.microsoft.com/config/ubuntu/"$ver"/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

#Install the .NET SDK
if [ "$ver" == "18.04" ]; then
	sudo add-apt-repository universe
fi
sudo apt-get install apt-transport-https
#sudo apt-get update
sudo apt-get install dotnet-sdk-2.2

dotnet run -p CRenderTest -c Release-Linux	#Compile and run
reset
clear