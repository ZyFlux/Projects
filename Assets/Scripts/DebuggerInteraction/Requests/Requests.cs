[System.Serializable]
public class Requests
{
    public string requestType;

}

public class Receive : Requests
{
    int receiverId;
}

public class StateQuerry : Requests
{
    int actorId;
}

public class TagActor : Requests
{
    int actorId;
    bool wantToTag; //True- Tag, False- Untag
}

