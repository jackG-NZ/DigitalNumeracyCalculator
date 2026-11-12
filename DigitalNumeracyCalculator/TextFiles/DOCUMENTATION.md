**BUGS**

The program currently doesn't handle multiple operators occuring one after the other, for example: 2 + + + + + 3
It doesn't crash, but it causes weird things to happen. Should be pretty easy to fix.

**AI USAGE**

I used AI to figure out why my shunting algorithm wasn't ordering ^ operators correctly. It said because ^ operators have right associativity instead of left, unlike all other operators.
It told me I could make an extra method to check each token's associativity but I decided this was redundant and instead just added an if statement.