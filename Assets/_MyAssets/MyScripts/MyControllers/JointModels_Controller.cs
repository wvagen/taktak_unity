
using System.Collections.Generic;
using System.Threading.Tasks;
using com.mkadmi;

public class JointModels_Controller
{
    static JointModels_Controller _instance;

    public static JointModels_Controller Instance()
    {
        if (_instance == null)
        {
            _instance = new JointModels_Controller();
        }
        return _instance;
    }

    public async Task<List<Joint_Company_MissionLive_Model>> GetMissionLiveWithCompanies(int limit, int offset)
    {
        var missionLiveList = new List<Joint_Company_MissionLive_Model>();

        var missionLives = await SB_Client.Instance()
            .From<MissionLive_Model>()
            .Select("*, company:company_id(*)")
            .Limit(limit)
            .Offset(offset)
            .Get();

        var company = await Company_Controller.Instance().GetMissionById(1);

        foreach (var missionLiveData in missionLives.Models)
        {
            var missionLiveWithCompany = new Joint_Company_MissionLive_Model
            {
                MissionLive = missionLiveData,
                Company = company,
            };
            missionLiveList.Add(missionLiveWithCompany);
        }

        return missionLiveList;
    }

}
