import re
import json
import subprocess
import os

data = []  # Stores each line from the log file as a JSON
philosophers = set()  # Stores a list of every philosopher actor
chopsticks = set()  # Stores a list of every chopstick actor

# Parse the data from the log file
with open('allEvents.txt') as f:
    for line in f:
        # Use Regex to find event, actors involved, and message in log file
        # Group 1 = action, group 2 = receiver, group 3 = sender, group 4 = message
        scan = re.match(r'^(\w+)\(Actor\[(.+?)\],Actor\[(.+)\],\(\d+,Envelope\((.+),Actor.+$', line)
        # Compile information into a JSON line and store it in data array
        if scan:
            # Parse actor names from directory path, e.g. "akka://DiningHakkers/user/philosopher-A#1134301393"
            receiver = scan.group(2).split("/")[-1].split("#")[0]
            sender = scan.group(3).split("/")[-1].split("#")[0]
            # Create and store JSON line from parsed tokens
            json_line = "{\"eventType\":\"" + scan.group(1) + "\",\"receiverId\":\"" + receiver + \
                "\",\"senderId\":\"" + sender + "\",\"msg\":\"" + scan.group(4) + "\"}"
            data.append(json.loads(json_line))
            # Add actors to their respective lists if they are not already included in the list
            if receiver.split("-")[0] == "philosopher":
                philosophers.add(receiver)
            elif receiver.split("-")[0] == "chopstick":
                chopsticks.add(receiver)
            if sender.split("-")[0] == "philosopher":
                philosophers.add(sender)
            elif sender.split("-")[0] == "chopstick":
                chopsticks.add(sender)
# Sort actors in alphabetical order by order of deadLetters, philosophers, and chopsticks
sorted_actors = sorted(philosophers) + sorted(chopsticks)
sorted_actors.insert(0, "deadLetters")

# Assign each actor its own MSCgen ascii character variable, starting from lower case letter 'a'
var = {}
alphabet = 97
for actor in sorted_actors:
    var[actor] = chr(alphabet)
    alphabet = alphabet + 1

# Create a new folder called "Outputs" to save all the output files
path = os.getcwd() + "/Outputs"
if not os.path.exists(path):
    os.makedirs(path)
os.chdir(path)

# Write the output to the MSCgen input file
output_file = open("output.txt", "w")  # create output file with all MSCgen commands
declaration = "msc {\nhscale = \"2\";\n"  # header statements to set up a new MSCgen file
# Declare list of actors based on assigned variable names
for actor in sorted_actors[:-1]:
    if actor == "deadLetters":
        declaration = declaration + var[actor] + " [label = \"\"],\n"  # make the deadLetters box nameless
    else:
        declaration = declaration + var[actor] + " [label = \"" + actor + "\"],\n"
else:
    last_actor = sorted_actors.pop()
    declaration = declaration + var[last_actor] + " [label = \"" + last_actor + "\"];\n"
output_file.write(declaration)
# Create output for step 0 with declarations
dec_file = open("Step0.txt", "w")
dec_file.write(declaration)
dec_file.write("}")
dec_file.close()
subprocess.call(["mscgen", "-o", "Step0.png", "-Ssignalling", "Step0.txt"])
# Write each JSON step (after step 0) to output file as a MSCgen connection between actors
count = 0  # keeps track of step number
for x in range(0, len(data)):
    if data[x]['eventType'] == "MessageReceivedByARef":  # if event is a message-received then create a new step
        # create a chart for each individual step
        if count > 0:
            step_file.write("}")
            step_file.close()
            subprocess.call(["mscgen", "-o", "Step" + str(count) + ".png", "-Ssignalling", "Step" + str(count) + ".txt"])
        count = count + 1
        step_file = open("Step" + str(count) + ".txt", "w")
        step_file.write(declaration)
        # write to both output and step files
        output_file.write("--- [label = \"Step " + str(count) + "\"];\n")
        output_file.write(var[data[x]['receiverId']] + " box " + var[data[x]['receiverId']] +
                          " [label = \"Message: " + data[x]['msg'] + "\\nSender: " + data[x]['senderId'] + "\"];\n")
        step_file.write("--- [label = \"Step " + str(count) + "\"];\n")
        step_file.write(var[data[x]['receiverId']] + " box " + var[data[x]['receiverId']] +
                        " [label = \"Message: " + data[x]['msg'] + "\\nSender: " + data[x]['senderId'] + "\"];\n")
    elif data[x]['eventType'] == "MessageSentByARef":  # if event is a message-sent then create a new connection
        # write to both output and step files
        output_file.write(var[data[x]['senderId']] + " -> " + var[data[x]['receiverId']] +
                          " [label = \"" + data[x]['msg'] + "\"];\n")
        if count > 0:
            step_file.write(var[data[x]['senderId']] + " -> " + var[data[x]['receiverId']] +
                            " [label = \"" + data[x]['msg'] + "\"];\n")
# write to final output and step file
step_file.write("}")
step_file.close()
# call MSCgen on the final step file
subprocess.call(["mscgen", "-o", "Step" + str(count) + ".png", "-Ssignalling", "Step" + str(count) + ".txt"])
output_file.write("}")
output_file.close()
# call MSCgen on the final output file to generate complete chart with all steps
subprocess.call(["mscgen", "-o", "All Steps.png", "-Ssignalling", "output.txt"])




