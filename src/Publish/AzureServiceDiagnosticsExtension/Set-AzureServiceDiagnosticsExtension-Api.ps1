$storage_name="jymstoretestapilogs"
$key="vsDPO9l8qZJ2vO7OlsPalOy14ooPwQqvf4EH2z34r3qQU6Ed86TAgaFzLmYdHvqA8smUgHwodpb10wZTlT82SA=="
$config_path="E:\io.yuyi.jinyinmao.server\src\Publish\AzureServiceDiagnosticsExtension\PaaSDiagnostics.Yuyi.Jinyinmao.Api.PubConfig.xml"
$service_name="jym-test-api"
$role="Yuyi.Jinyinmao.Api"
$storageContext=New-AzureStorageContext -StorageAccountName $storage_name -StorageAccountKey $key 
$slot="Production"
Set-AzureServiceDiagnosticsExtension -StorageContext $storageContext -DiagnosticsConfigurationPath $config_path -ServiceName $service_name -Slot $slot -Role $role