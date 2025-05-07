// This is a basic example
== IntroductoryScene ===
You awaken in a garden.
You don’t remember how you got here. 
There's a gate, but it's shut tight. 
To leave, you must listen, explore, and unlock the secrets hidden in each corner of the garden.
Collect the trophy to open the gate.
Your journey begins here.
 -> DONE


VAR hitBullseyes = false
VAR givenTrophy = false
VAR movement = ""

== TalkToNPC1 ==

{hitBullseyes:
    Here is your trophy!
    ~ givenTrophy = true
    -> DONE
}

{givenTrophy:
    I already gave you your trophy!
    -> DONE
}

I will grant you a trophy if you can hit three bullseyes. 

How would you like the target to move?
    * Sinusoidally
     ~ movement = "sinusoidal"
    -> DONE
    * Linearly
     ~ movement = "linear"
    -> DONE

-> DONE
