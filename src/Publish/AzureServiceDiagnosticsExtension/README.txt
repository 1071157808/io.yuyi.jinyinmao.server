
// 先要配置PowerShell的Azure订阅

$storage_name 			// 存储日志的Storage名称
$key  					// 存储日志的Storage私钥
$config_path			// PubConfig.xml的绝对路径，不能使用相对路径
$service_name			// 要配置的CloudService名称
$role 					// 要配置的Role名称
$storageContext=New-AzureStorageContext -StorageAccountName $storage_name -StorageAccountKey $key 					 // 该行不需要修改
$slot 					// 环境名称
Set-AzureServiceDiagnosticsExtension -StorageContext $storageContext -DiagnosticsConfigurationPath $config_path -ServiceName $service_name -Slot $slot -Role $role // 该行不需要修改