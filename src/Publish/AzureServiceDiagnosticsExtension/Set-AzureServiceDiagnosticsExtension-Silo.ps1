$storage_name="jymstoredevsilologs"
$key="vWfjV2TOd7nEsO/5ekCpYWge3b+WWp248Jzv8qjZn4yMsqbrIyy3MJ3eLVwoi06sczGi/vQaBgfLK/ZqHJE3fg=="
$config_path="C:\Users\Siqi\Desktop\AzureServiceDiagnosticsExtension\PaaSDiagnostics.Yuyi.Jinyinmao.Silo.PubConfig.xml"
$service_name="jym-dev-api"
$role="Yuyi.Jinyinmao.Silo"
$storageContext=New-AzureStorageContext -StorageAccountName $storage_name -StorageAccountKey $key 
$slot="Production"
Set-AzureServiceDiagnosticsExtension -StorageContext $storageContext -DiagnosticsConfigurationPath $config_path -ServiceName $service_name -Slot $slot -Role $role