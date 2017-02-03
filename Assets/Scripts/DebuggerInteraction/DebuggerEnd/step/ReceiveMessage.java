package step;

import org.json.JSONObject;

public class ReceiveMessage extends Step {
    String actorId;
    // to add more fields?

    public ReceiveMessage(String actorId) {
        super(StepType.RECEIVE_MESSAGE);
        this.actorId = actorId;
    }

    public ReceiveMessage(JSONObject jsonEvent) {
        super(StepType.RECEIVE_MESSAGE);
        this.actorId = jsonEvent.optString("actorId", "");
    }

    public JSONObject toJson() throws Exception {
        JSONObject json = super.toJson();
        json.put("actorId", actorId);
        return json;
    }
}
