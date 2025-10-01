# OrdersApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**apiV1OrdersGet**](#apiv1ordersget) | **GET** /api/v1/orders | |
|[**apiV1OrdersOrderNumberGet**](#apiv1ordersordernumberget) | **GET** /api/v1/orders/{orderNumber} | |
|[**apiV1OrdersOrderNumberStatusPatch**](#apiv1ordersordernumberstatuspatch) | **PATCH** /api/v1/orders/{orderNumber}/status | |
|[**apiV1OrdersPost**](#apiv1orderspost) | **POST** /api/v1/orders | |

# **apiV1OrdersGet**
> Array<OrderDto> apiV1OrdersGet()

Get orders by filter.

### Example

```typescript
import {
    OrdersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new OrdersApi(configuration);

let limit: number; // (default to undefined)
let offset: number; // (optional) (default to undefined)
let freeText: string; // (optional) (default to undefined)
let orderStatus: Array<1 | 2 | 3 | 4>; // (optional) (default to undefined)
let createdFrom: string; // (optional) (default to undefined)
let createdTo: string; // (optional) (default to undefined)
let updatedFrom: string; // (optional) (default to undefined)
let updatedTo: string; // (optional) (default to undefined)
let orderBy: 1 | 2 | 3 | 4; // (optional) (default to undefined)

const { status, data } = await apiInstance.apiV1OrdersGet(
    limit,
    offset,
    freeText,
    orderStatus,
    createdFrom,
    createdTo,
    updatedFrom,
    updatedTo,
    orderBy
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **limit** | [**number**] |  | defaults to undefined|
| **offset** | [**number**] |  | (optional) defaults to undefined|
| **freeText** | [**string**] |  | (optional) defaults to undefined|
| **orderStatus** | **Array<1 &#124; 2 &#124; 3 &#124; 4>** |  | (optional) defaults to undefined|
| **createdFrom** | [**string**] |  | (optional) defaults to undefined|
| **createdTo** | [**string**] |  | (optional) defaults to undefined|
| **updatedFrom** | [**string**] |  | (optional) defaults to undefined|
| **updatedTo** | [**string**] |  | (optional) defaults to undefined|
| **orderBy** | [**1 | 2 | 3 | 4**]**Array<1 &#124; 2 &#124; 3 &#124; 4>** |  | (optional) defaults to undefined|


### Return type

**Array<OrderDto>**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **apiV1OrdersOrderNumberGet**
> OrderDto apiV1OrdersOrderNumberGet()

Get order by id.

### Example

```typescript
import {
    OrdersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new OrdersApi(configuration);

let orderNumber: string; // (default to undefined)

const { status, data } = await apiInstance.apiV1OrdersOrderNumberGet(
    orderNumber
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **orderNumber** | [**string**] |  | defaults to undefined|


### Return type

**OrderDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **apiV1OrdersOrderNumberStatusPatch**
> OrderDto apiV1OrdersOrderNumberStatusPatch(updateOrderStatusCommand)

Update order.

### Example

```typescript
import {
    OrdersApi,
    Configuration,
    UpdateOrderStatusCommand
} from './api';

const configuration = new Configuration();
const apiInstance = new OrdersApi(configuration);

let orderNumber: string; // (default to undefined)
let updateOrderStatusCommand: UpdateOrderStatusCommand; //

const { status, data } = await apiInstance.apiV1OrdersOrderNumberStatusPatch(
    orderNumber,
    updateOrderStatusCommand
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **updateOrderStatusCommand** | **UpdateOrderStatusCommand**|  | |
| **orderNumber** | [**string**] |  | defaults to undefined|


### Return type

**OrderDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **apiV1OrdersPost**
> OrderDto apiV1OrdersPost(createOrderCommand)

Create order.

### Example

```typescript
import {
    OrdersApi,
    Configuration,
    CreateOrderCommand
} from './api';

const configuration = new Configuration();
const apiInstance = new OrdersApi(configuration);

let createOrderCommand: CreateOrderCommand; //

const { status, data } = await apiInstance.apiV1OrdersPost(
    createOrderCommand
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **createOrderCommand** | **CreateOrderCommand**|  | |


### Return type

**OrderDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

