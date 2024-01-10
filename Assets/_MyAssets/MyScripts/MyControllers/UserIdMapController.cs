using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;

public class UserIdMapController
{

    public async Task<List<UserIdMap>> GetAllUserIdMaps()
    {
        var response = await SB_Client.Instance()
            .From<UserIdMap>()
            .Select("*")
            .Get();

        return response.Models;
    }

    public async Task<UserIdMap> GetUserIdMapById(long id)
    {
        var response = await _supabaseClient
            .From<UserIdMap>()
            .Match(id)
            .Select("*")
            .GetSingleAsync();

        return response.Data;
    }

    public async Task<UserIdMap> CreateUserIdMap(UserIdMap userIdMap)
    {
        var response = await _supabaseClient
            .From<UserIdMap>()
            .UpSert(userIdMap)
            .GetSingleAsync();

        return response.Data;
    }

    public async Task<UserIdMap> UpdateUserIdMap(UserIdMap userIdMap)
    {
        var response = await _supabaseClient
            .From<UserIdMap>()
            .UpSert(userIdMap)
            .GetSingleAsync();

        return response.Data;
    }

    public async Task<bool> DeleteUserIdMap(long id)
    {
        var response = await _supabaseClient
            .From<UserIdMap>()
            .Match(id)
            .DeleteAsync();

        return response.Status == 200;
    }
}
