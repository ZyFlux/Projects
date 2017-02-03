package step;

import org.json.JSONObject;

public class ChangeState extends Step {

    String actorId;
    String toState;

    public ChangeState(String actorId, String toState) {
        super(StepType.CHANGE_STATE);
        this.actorId = actorId;
        this.toState = toState;
    }

    public ChangeState(JSONObject jsonEvent) {
        super(StepType.CHANGE_STATE);
        this.actorId = jsonEvent.optString("actorId", "");
        this.toState = jsonEvent.optString("toState", "");
    }

    public JSONObject toJson() throws Exception {
        JSONObject json = super.toJson();
        json.put("actorId", actorId).put("toState", toState);
        return json;
    }

}
