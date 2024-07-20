﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    public abstract class ScoreBase : NativeObjectBase, IScore
    {
        #region Constructors

        protected ScoreBase(string leaderboardId, string leaderboardPlatformId)
        {
            // set properties
            LeaderboardId           = leaderboardId;
            LeaderboardPlatformId   = leaderboardPlatformId;
        }

        protected ScoreBase(string leaderboardPlatformId)
        {
            var     settings        = GameServices.FindLeaderboardDefinitionWithPlatformId(leaderboardPlatformId);
            Assert.IsFalse(null == settings, "Could not find settings for specified platform id: " + leaderboardPlatformId);
            
            // set properties
            LeaderboardId           = settings.Id;
            LeaderboardPlatformId   = leaderboardPlatformId;
        }

        #endregion

        #region Abstract methods

        protected abstract IPlayer GetPlayerInternal();

        protected abstract long GetRankInternal();

        protected abstract long GetValueInternal();

        protected abstract void SetValueInternal(long value);

        protected abstract DateTime GetLastReportedDateInternal();

        protected abstract void ReportScoreInternal(ReportScoreInternalCallback callback);

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("Score { ");
            sb.Append("LeaderboardId: ").Append(LeaderboardId).Append(" ");
            sb.Append("Value: ").Append(Value).Append(" ");
            sb.Append("Rank: ").Append(Rank).Append(" ");
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IGameServicesScore implementation

        public string LeaderboardId { get; internal set; }

        public string LeaderboardPlatformId { get; internal set; }

        public IPlayer Player => GetPlayerInternal();

        public long Rank => GetRankInternal();

        public long Value
        {
            get => GetValueInternal();
            set => SetValueInternal(value);
        }

        public string FormattedValue => GetValueInternal().ToString();

        public DateTime LastReportedDate => GetLastReportedDateInternal();

        public void ReportScore(CompletionCallback callback)
        {
            // retain object to avoid unintentional releases
            ManagedObjectReferencePool.Retain(this);

            // make native call
            ReportScoreInternal((success, error) =>
            {
                // send result to caller object
                CallbackDispatcher.InvokeOnMainThread(callback, success, error);

                // remove object from cache
                ManagedObjectReferencePool.Release(this);
            });
        }

        [Obsolete("This method is deprecated. Use ReportScore(CompletionCallback) method instead.", false)]
        public void ReportScore(CompletionCallbackLegacy callback)
        {
            ReportScore((success, error) =>
            {
                callback?.Invoke(error);
            });
        }

        #endregion
    }
}