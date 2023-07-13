using Extensions;
using KickStreaming.User.Models;
using System.Text.Json;
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable CS8602

namespace KickStreaming.User; 

public class KickUser {
    
    private readonly KickApi _kickApi;
    
    internal KickUser(KickApi kickApi) => _kickApi = kickApi;
    
    /// <summary>
    /// Gets the channel info
    /// </summary>
    /// <param name="channel">Username/channel name</param>
    /// <returns>Returns the channel info</returns>
    public async Task<ChannelInfo?> GetChannelInfo(string channel) {
        var response = await _kickApi.CycleTls.SendAsync(KickApi.GetSpoofOptions($"channels/{channel}", 2, HttpMethod.Get, new Dictionary<string, string>()));
        if (response.Status == 200)
            return JsonSerializer.Deserialize<ChannelInfo>(response.Body) ?? null;
        _kickApi.Logger.Log<KickApi>($"[API Error][GetChannelInfo]: {response.Body}", Logger.Level.Warning);
        return null;
    }

    /// <summary>
    /// Gets the channel id
    /// </summary>
    /// <param name="channel">Username/channel name</param>
    /// <returns>Returns the channel numeric id</returns>
    public async Task<int> GetChannelId(string channel) =>
        (await GetChannelInfo(channel)).Id;
    
    /// <summary>
    /// Gets the channel chatroom id
    /// </summary>
    /// <param name="channel">Username/channel name</param>
    /// <returns>Returns the chatroom numeric id</returns>
    public async Task<int> GetChatroomId(string channel) =>
        (await GetChannelInfo(channel)).Chatroom.Id;

}