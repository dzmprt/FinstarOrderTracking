# AppInfoApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**version**](#version) | **GET** /app-info/version | |
|[**whoAmI**](#whoami) | **GET** /app-info/who-am-i | |

# **version**
> version()

Version

### Example

```typescript
import {
    AppInfoApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AppInfoApi(configuration);

const { status, data } = await apiInstance.version();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **whoAmI**
> whoAmI()

Who am i

### Example

```typescript
import {
    AppInfoApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AppInfoApi(configuration);

const { status, data } = await apiInstance.whoAmI();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

