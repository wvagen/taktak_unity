using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace com.mkadmi
{
    public class ApiClient : MonoBehaviour
    {
        private static ApiClient _instance;
        public static ApiClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ApiClient");
                    _instance = go.AddComponent<ApiClient>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private HttpClient _httpClient;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                _httpClient = new HttpClient();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async Task<ApiResponse> GetAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonUtility.FromJson<ApiResponse>(jsonResponse);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.LogError($"Error: {response.StatusCode}, Details: {errorResponse}");
                    return new ApiResponse { code = (int)response.StatusCode, message = errorResponse };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {e.Message}");
                return new ApiResponse { code = 500, message = e.Message };
            }
        }

        public async Task<ApiResponse> PostAsync<T>(string url, T payload)
        {
            Debug.LogFormat("URL{0} : PayLoad:{1}", url,payload);
            return await SendAsync(HttpMethod.Post, url, payload);
        }

        public async Task<ApiResponse> PutAsync<T>(string url, T payload)
        {
            return await SendAsync(HttpMethod.Put, url, payload);
        }

        public async Task<ApiResponse> PatchAsync<T>(string url, T payload)
        {
            return await SendAsync(new HttpMethod("PATCH"), url, payload);
        }

        public async Task<ApiResponse> DeleteAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonUtility.FromJson<ApiResponse>(jsonResponse);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.LogError($"Error: {response.StatusCode}, Details: {errorResponse}");
                    return new ApiResponse { code = (int)response.StatusCode, message = errorResponse };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {e.Message}");
                return new ApiResponse { code = 500, message = e.Message };
            }
        }

        private async Task<ApiResponse> SendAsync<T>(HttpMethod method, string url, T payload)
        {
            try
            {
                Debug.Log("PayLoad before converting: " + payload); 
                string jsonData = JsonUtility.ToJson(payload);
                Debug.Log("jsonData: " + jsonData);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(method, url) { Content = content };
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonUtility.FromJson<ApiResponse>(jsonResponse);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.LogError($"Error: {response.StatusCode}, Details: {errorResponse}");
                    return new ApiResponse { code = (int)response.StatusCode, message = errorResponse };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {e.Message}");
                return new ApiResponse { code = 500, message = e.Message };
            }
        }
    }

    [Serializable]
    public class ApiResponse
    {
        public int code;
        public string message;
        public object data;
    }
}
