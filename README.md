# VBML
Some mad machine learning tings in the most hipster language known to man: Visual Basic.

Currently Implements:
- Logistic Regression

Will Probably Implement Sometime in the Distant Distant Future
- Linear Regression
- ANNs/DNNs
- SVMs and other exciting Kernel things
- Multivariate stuff and reguarlisation
- K-Means/Modes
- Boltzmann Machines
- DBNs

##Preface
Machine Learning. What languages come to mind? R? Python? Matlab? Bet you didn't think Visual Basic. 

There are probably reasons as to why few have even bothered building open-source machine learning applications in VB (along the lines of uselessness, crappy performance and a general lack of VB people out there).
I have, however, in an intoxicated, sleep-deprived and overly-confident state, set out on this gargantuan (and probably pointless) exercise to teach a friend (who we shall call Hiroshige) something ever-so-slightly beyond the scope of the Computer Science GCSE syllabus.

I have neither any qualifications in Computer Science nor am I familiar with VB. Feel free to fix any horrible errors I've made in my incredibly unstable mental state.

##Demos

NB: If you want to use your own data, make sure you've saved it as a .CSV file (you can do this in Excel) and make sure that
- The output values are in the last column (green box)
- You have a headings row (red box)
- Boolean values are represented as 1s or 0s (green box)

![alt text](http://i.imgur.com/4BtcjLF.png "Formatting Excel")

###Logistic Classifier

This demo takes in training examples and trains a binary (yes/no) classifier on them. This can be vaguely useful in some situations - for example, if you're selling glow-in-the-dark towels, you can use a logistic classifier to work out which of your customers would be interested in your new range of Disney-themed bathroom essentials based upon their previous purchases.

The demo looks a bit like this:
![alt text](http://i.imgur.com/fiibQrf.png "Logistic Classifier")

Either enter a number or type a path to a .csv file to begin training a classifier. You'll then be allowed to give it inputs to make predictions on.

![alt text](http://i.imgur.com/bjlBzuF.png "Prediction")
