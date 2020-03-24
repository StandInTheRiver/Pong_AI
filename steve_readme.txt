AI_PONG_PROJECT - this is the blank unity project we will use to create pong
		- It has the ml agents package installed but nothing else

ml-agents-0.xxx - This is the cloned verison from the github for ml agents, it has everyhting you need
		- It has a subfolder called projects which have example projects setup you can view and mess around with
		- to open the samples just open the folder "project" in unity hub and run it with unity


Version for Unity for our project and for the sample projects is = 2018.4.18f1


Steps for installing

1) download unity hub
2) create account/all that nonsense
3) login
4) navigate to https://unity3d.com/get-unity/download/archive
5) find the 2018.4.18 version, click the green unity hub icon to install
6) once that's done, you can add our project to unity hub and than click it to open unity


In order to use Ml-Agents you gotta do this

1) Install python version 3.7.7 https://www.python.org/downloads/
2) ensure you click advanced options and select "include windows Path" or else your cmd commands wont work
3) open cmd elevated, cd until you are in the folder ml-agents
4) run the following commands in order 
cd ml-agents-envs
pip3 install -e ./
cd ..
cd ml-agents
pip3 install -e ./
5) now run pip3 install mlagents
to test you can type mlagents-learn --help and you should see some commands


in order to test out the funcitonality of ml-agents so far i've done this

1) open the project example in unity
2) navigate to 3dball under examples, open 3d ball scene
3) should see cubes now
4) click the arrow to view the prefab for any 3dball object in the hierachy panel
5)select agent object
6) in the inspector find behavior parameters script
7) find the model field
8)click the o to bring up list
9) set it to none

click play
Balls should spawn randomly on top of the head and each head should spawn in a random tilt, the ball falls off than it resets randomly again
I think this is the behaviour when no "brain" is given to the ai

Now you can go back to the prefab menu and assign "3dball_steve"
This is the brain i generated earlier, assign it and click play
You will now see the cubes behave accordingly 


In order to generate new Ai "brain" follow these steps

1) with unity open, and not playing, open elevated CMD
2) navigate to the folder structure pong_ai\ml-agents-0.15.0\ml-agents
3) run this command "mlagents-learn config/trainer_config.yaml --run-id=runnamex --train
		mlagents-learn is the command
		config/ - specifies an algorthim to use
		trainer_config.yaml is the algorthim
		--run-id= - this is where you name the training run
		--train is the command to execute train
4) you should see some shit come up and your command prompt  wait
5) now click play on unity
6) you should see the cubes now learning how to juggle the ball at x100 speed
7) in cmd you will see some info, when it finshes its iterations it outputs a file
8) take that file and place it into unity, than use that file the same way we did earlier ( its the brain ) to apply it to your cubes



