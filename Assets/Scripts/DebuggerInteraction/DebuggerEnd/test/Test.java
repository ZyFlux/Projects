package test;

import org.json.JSONObject;
import step.*;

public class Test {

    public static void main(String[] args) {

        CreateActor step1 = new CreateActor("Actor1");
        SendMessage step2 = new SendMessage("Actor1", "Actor2", "dummyMessage");
        ReceiveMessage step3 = new ReceiveMessage("Actor5");
        ChangeState step4 = new ChangeState("Actor10", "state1");

        String step1Str = "", step2Str = "", step3Str = "", step4Str = "";

        try {
            step1Str = step1.toJson().toString();
            step2Str = step2.toJson().toString();
            step3Str = step3.toJson().toString();
            step4Str = step4.toJson().toString();
        } catch (Exception e) {
            System.out.println(e);
        }

        try {
            Step readStep1 = Step.createStep(new JSONObject(step1Str));
            Step readStep2 = Step.createStep(new JSONObject(step2Str));
            Step readStep3 = Step.createStep(new JSONObject(step3Str));
            Step readStep4 = Step.createStep(new JSONObject(step4Str));

            System.out.println(readStep1.toString());
            System.out.println(readStep2.toString());
            System.out.println(readStep3.toString());
            System.out.println(readStep4.toString());

        } catch (Exception e) {
            System.out.println(e);
        }

    }
}
