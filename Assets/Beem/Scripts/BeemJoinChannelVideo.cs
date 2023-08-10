using System.Collections;
using System.Collections.Generic;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;
using Agora.Rtc;
using UnityEngine;

public class BeemJoinChannelVideo : JoinChannelVideo
{
    [SerializeField] private bool _clientRoleBroadcaster;
    [SerializeField] private Transform _videoParent;

    protected override void SetBasicConfiguration()
    {
        RtcEngine.EnableAudio();
        RtcEngine.EnableVideo();
        var config = new VideoEncoderConfiguration
        {
            dimensions = new VideoDimensions(640, 360),
            frameRate = 15,
            bitrate = 0
        };
        
        RtcEngine.SetVideoEncoderConfiguration(config);
        RtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);

        RtcEngine.SetClientRole(_clientRoleBroadcaster
            ? CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER
            : CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
    }

    [ContextMenu("Join")]
    public override void JoinChannel()
    {
        RtcEngine.JoinChannel(Token, ChannelName);
        // MakeVideoView(0, onPlane: true, parent: _videoParent);
    }

    protected override void InitEngine()
    {
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        var handler = new UserEventHandler(this, _videoParent, Prefab);
        var context = new RtcEngineContext(AppID, 0,
            CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
            AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        RtcEngine.Initialize(context);
        RtcEngine.InitEventHandler(handler);
    }

    private class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinChannelVideo _videoSample;
        private readonly Transform _videoParent;
        private readonly GameObject _prefab;
        
        internal UserEventHandler(JoinChannelVideo videoSample, Transform videoParent, GameObject prefab)
        {
            _videoSample = videoSample;
            _videoParent = videoParent;
            _prefab = prefab;
        }

        public override void OnError(int err, string msg)
        {
            _videoSample.Log.UpdateLog($"OnError err: {err}, msg: {msg}");
        }

        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            var build = 0;
            Debug.Log("Agora: OnJoinChannelSuccess ");
            _videoSample.Log.UpdateLog($"sdk version: ${_videoSample.RtcEngine.GetVersion(ref build)}");
            _videoSample.Log.UpdateLog($"sdk build: ${build}");
            _videoSample.Log.UpdateLog($"OnJoinChannelSuccess channelName: {connection.channelId}, uid: {connection.localUid}, elapsed: {elapsed}");
        }

        public override void OnRejoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            _videoSample.Log.UpdateLog("OnRejoinChannelSuccess");
        }

        public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
        {
            _videoSample.Log.UpdateLog("OnLeaveChannel");
        }

        public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole, ClientRoleOptions newRoleOptions)
        {
            _videoSample.Log.UpdateLog("OnClientRoleChanged");
        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            _videoSample.Log.UpdateLog($"OnUserJoined uid: ${uid} elapsed: ${elapsed}");
            MakeVideoView(uid, _videoSample.GetChannelName(), true, _videoParent, _prefab);
        }

        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.Log.UpdateLog($"OnUserOffLine uid: ${uid}, reason: ${(int)reason}");
            DestroyVideoView(uid);
        }

        public override void OnUplinkNetworkInfoUpdated(UplinkNetworkInfo info)
        {
            _videoSample.Log.UpdateLog("OnUplinkNetworkInfoUpdated");
        }

        public override void OnDownlinkNetworkInfoUpdated(DownlinkNetworkInfo info)
        {
            _videoSample.Log.UpdateLog("OnDownlinkNetworkInfoUpdated");
        }
    }
}
