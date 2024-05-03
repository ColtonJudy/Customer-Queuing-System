# Customer Queuing System

Developed by Colton Judy and John Steltzner

Busy lines in stores can be off-putting and unappealing to customers, making the trip to the store a frustrating experience when trying to checkout. The problem of inefficient checkout processes affects not only the individual store, but also that store’s organization.

The Customer Queuing System(CQS) is designed to help stores minimize checkout traffic by utilizing queuing systems for available registers and self-checkouts.

For more information on how the CQS works, Check out our documentation below.

 ## Documentation
 - [Vision Document](https://github.com/ColtonJudy/Customer-Queuing-System/blob/master/Documents/Software%20Engineering%20Vision%20Document.pdf)
 - [Requirements](https://github.com/ColtonJudy/Customer-Queuing-System/blob/master/Documents/CQS%20Requirements.pdf)
 - [Scenarios](https://github.com/ColtonJudy/Customer-Queuing-System/blob/master/Documents/scenarios.pdf)
 - [Use Case Diagram](https://github.com/ColtonJudy/Customer-Queuing-System/blob/master/Documents/use-cases.drawio.png)
 - [Class Diagram](https://github.com/ColtonJudy/Customer-Queuing-System/blob/master/Documents/ClassDiagram.drawio.png)

## Features
- Ability for the user to select self checkout or standard checkout
- Ability to choose more or less than 15 items for express checkout eligibility 
- Ability to choose cash or card
- Automatic queuing system
- Detection for when a customer finishes checking out triggered by finishing purchase alongside multiple purchase prompts at POSs 
- Detection for whenever an employee opens or closes an aisle 
- Identification of the statuses of POSs: Open, Closed, Delayed 
- Recommendation to open up POS if queues are full
- Setup wizard that allows store managers to set up the program with a custom png or jpg logo, as well as determine the amount of registers, their types, and whether they are cash/card only
- Testing simulation that simulates the queuing system in real time.
- Return of checkout data daily upon closing of the kiosk

All features that require integration with hardware can be tested using the robust simulation mode that can be enabled in the config.

## Known Issues

Minor bugs with express lane capacity.

## Future Additions
- Improved UI
- A feature where the user can select from a number of recommendation options according to their preference, instead of being forced to choose the best one available.
