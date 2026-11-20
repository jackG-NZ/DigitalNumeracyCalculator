**BUGS**

1.
	The calculator currently doesn't handle multiple operators occuring one after the other, for example: 2 + + + + + 3.
	It doesn't crash, but it should return an error message or something of the sorts letting the user know this is invalid.
2.  
	After the bad expression message, all inputs excluding "ex" are ignored. For example:
	Bad expression. Input "ex" to see an example of a valid expression.
    mat a 1 2 3 4
	The matrix is not initialized because the input is not ignored. Should fix this at some point. (FIXED)
3.
	While navigating the sub-menus, it's possible for the user to input something other than "q" which may take them to a different sub-menu.
	This isn't horrible, but it might be a bit confusing for the user because they may wonder where they are and how they got there. The idea
	is that all sub-menus are accessible through via the "main" menu and should not be able to access each other. This makes menu navigation
	very linear but for a project of this size I don't think it should be a problem.
4.
	Encountered issues setting up some test methods. This is explained in more detail in TestGenerics.cs.
5.
	The calculator currently doesn't handle arithmetic operations of decimals, for example: 2.5 + 2.5. I will need to redesign the shunting-yard
	algorithm to accommodate for decimal nums. It doesn't crash but it returns an error message if the user attempts to operate on decimal
	numbers, which is a bit silly (what kind of calculator can't handle 2.5 + 2.5???)
6.
	If the user attempts to perform an operation on a struct that doesn't exist, for example "midpointLine a" where "a" hasn't been initialized,
	the program will print two different error messages (one for a syntax error, another for a failed dereference) which might be a bit confusing.
	Not a bug, moreso something that should be changed to improved the user experience.

**AI USAGE**

1. 
	I used Copilot to figure out why my shunting algorithm wasn't ordering ^ operators correctly. It said because ^ operators have right associativity instead of left, unlike all other operators.
	It told me I could make an extra method to check each token's associativity but I decided this was redundant and instead just added an if statement to check for any occurances of ^.
2.
	I asked Copilot to verify that my InvMat method was returning the right results, which it did. It also suggested that I add protection for singular matrices (where the inverse does not exist).
	Copilot: "One extra test I'd recommend is a singular matrix, such as: [1 2 2 4]. Here the determinant is 1 * 4 - 2 * 2 = 0 so the inverse does not exist. Your invmat command should detect that and
	refuse the operation instead of dividing by zero. That's a great edge case to verify before calling the implementation finished."
	This was a good suggestion from Copilot as I had forgotten about this edge case.
3.
	I asked Copilot to verify that my NumRand method was generating the correct numbers for a given input. I sent a screenshot of the program's output and asked "Does this look correct?".
	Copilot: "Yes, that looks correct for the linear congruential generator xn+1 = (18xn + 3) mod 89 with seed x0 = 2." This was helpful because now I don't have to rely on myself to validate
	my own calculations and can instead have a third-party look through it and reassure me that I'm not making things up.
4.
	I asked Copilot for clarification about the fifth task in cryptography. I sent a screenshot of the task and wrote "Does this mean I would have to write a for loop that attempts decryptions
	over and over again while changing the values of a and b?". Copilot replied "Yes, that's typically how a brute-force attack on an Affine cypher works." It then provided a pseudo-code snippet
	of a brute force algorithm which I didn't end up using, but was somewhat helpful to get an idea of what the structure should look like.
