#####################################################
# Debugging stuff
#####################################################
def stap!
	require 'ruby-debug'; debugger;
end

Then /^ST[OA]P$/ do
	stap!
end
