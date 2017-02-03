package step;

import org.json.JSONObject;

public abstract class Step {

    public enum StepType {
        CREATE_ACTOR, RECEIVE_MESSAGE, SEND_MESSAGE, CHANGE_STATE
    }

    public final StepType type;

    public Step(StepType type) {
        this.type = type;
    }

    public static Step createStep(JSONObject jsonObj) {
        StepType type = StepType.valueOf(jsonObj.optString("type", "INVALID"));
        Step step = null;

        switch(type) {
            case CREATE_ACTOR:
                step = new CreateActor(jsonObj);
                break;
            case SEND_MESSAGE:
                step = new SendMessage(jsonObj);
                break;
            case RECEIVE_MESSAGE:
                step = new ReceiveMessage(jsonObj);
                break;
            case CHANGE_STATE:
                step = new ChangeState(jsonObj);
                break;
            default:
                // System.out.println("Unrecognized step type");
                break;
        }

        return step;
    }

    public JSONObject toJson() throws Exception {
        JSONObject json = new JSONObject();
        json.put("type", type.name());
        return json;
    }

    public String toString() {
        return String.format("%s", type.name());
    }
}
