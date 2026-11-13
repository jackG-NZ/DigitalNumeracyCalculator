**BUGS**

The program currently doesn't handle multiple operators occuring one after the other, for example: 2 + + + + + 3
It doesn't crash, but it causes weird things to happen. Should be pretty easy to fix.

**AI USAGE**

1. 
	I used Copilot to figure out why my shunting algorithm wasn't ordering ^ operators correctly. It said because ^ operators have right associativity instead of left, unlike all other operators.
	It told me I could make an extra method to check each token's associativity but I decided this was redundant and instead just added an if statement.

2.
	I asked Copilot to verify that my InvMat method was returning the right results, which it did. It also suggested that I add protection for singular matrices (where the inverse does not exist).
	Copilot: "One extra test I'd recommend is a singular matrix, such as: [1 2 2 4]. Here the determinant is 1 * 4 - 2 * 2 = 0 so the inverse does not exist. Your invmat command should detect that and
	refuse the operation instead of dividing by zero. That's a great edge case to verify before calling the implementation finished."
	This was a good suggestion from Copilot as I had forgotten about this edge case.