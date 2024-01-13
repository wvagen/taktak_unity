using Supabase;

namespace com.mkadmi
{
    public class SB_Client
    {
        static Client _supabaseClient;

        public static Client Instance()
        {
            if (_supabaseClient == null)
            {
                var url = Config.MY_SUPABASE_URL;
                var key = Config.MY_SUPABASE_KEY;

                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = true,
                };

                _supabaseClient = new Client(url, key, options);
                _supabaseClient.InitializeAsync();
            }

            return _supabaseClient;
        }


    }
}