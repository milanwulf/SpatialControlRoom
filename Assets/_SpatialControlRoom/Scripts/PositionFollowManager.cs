using System.Collections.Generic;
using UnityEngine;

public class PositionFollowManager : MonoBehaviour
{
    private List<UiFeed> registeredFollowers = new List<UiFeed>();
    private UiFeed activeFollower;

    public void RegisterPosFollower(UiFeed follower)
    {
        if (!registeredFollowers.Contains(follower))
        {
            registeredFollowers.Add(follower);
        }
    }

    public void DeregisterPosFollower(UiFeed follower)
    {
        if (registeredFollowers.Contains(follower))
        {
            registeredFollowers.Remove(follower);
            if (activeFollower == follower)
            {
                activeFollower = null;
            }
        }
    }

    public bool IsActiveFollower(UiFeed follower)
    {
        return activeFollower == follower;
    }

    public void RequestFollowActivation(UiFeed requester)
    {
        if (activeFollower != null)
        {
            if (activeFollower != requester)
            {
                activeFollower.DeactivatePositionFollowing();
                activeFollower = requester;
                activeFollower.ActivatePositionFollowing();
            }
        }
        else
        {
            activeFollower = requester;
            activeFollower.ActivatePositionFollowing();
        }
    }

    public void DeactivatePositionFollowing(UiFeed follower)
    {
        if (activeFollower == follower)
        {
            activeFollower = null;
        }
    }
}
