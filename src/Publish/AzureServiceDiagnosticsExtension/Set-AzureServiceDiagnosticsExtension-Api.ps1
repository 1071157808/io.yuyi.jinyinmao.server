$storage_name="jymstoredevapilogs"
$key="mo6gT9g0K+uykNlw0+y9D1FwWTecdPyeVrnLM+8Ow4cVpyCxwovunMAMvFc0k2t9lUuc/P+VV9H0E3Xj9HP2fw=="
$config_path="C:\Users\Siqi\Desktop\AzureServiceDiagnosticsExtension\PaaSDiagnostics.Yuyi.Jinyinmao.Api.PubConfig.xml"
$service_name="jym-dev-api"
$role="Yuyi.Jinyinmao.Api"
$storageContext=New-AzureStorageContext -StorageAccountName $storage_name -StorageAccountKey $key 
$slot="Production"
Set-AzureServiceDiagnosticsExtension -StorageContext $storageContext -DiagnosticsConfigurationPath $config_path -ServiceName $service_name -Slot $slot -Role $role