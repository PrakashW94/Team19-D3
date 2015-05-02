var selectedItems = [];
	$(document).ready(function ()		// Execute all of this on load 
	{	
	    // Add week selector	
	    
		var numSelecting = 0;		// Keep count of how many elements we have selected during selecting event
		var selectAll = false;
		var removeAll = false;
		setSelectedWeeks($("#Weeks").val());

		$("#weekSelector").bind("mousedown", function(e) {				
			e.metaKey = true;		// Simulates holding down control to select non-adjacent elements
		}).selectable(
		{	
			selecting: function(e, ui)
			{
				var clickedText = ui.selecting.innerHTML;
		
				if (numSelecting == 0)		// If this is the first element being selected
				{
					// If element is not already selected, we now add all other elements we later select
					if (selectedItems.indexOf(clickedText) == -1)
					{
						selectAll = true;
						removeAll = false;
					}
					else	// If element is already selected, then we remove all other elements we later select
					{
						selectAll = false;
						removeAll = true;
					}
				}						
		
				// If element is not selected, add it to array
				if (selectAll)
				{
					selectedItems.push(clickedText);
				}
		
				if (removeAll)	// If element is already selected, then remove it from the array and remove it's class
				{
					selectedItems.splice(selectedItems.indexOf(clickedText), 1);
					ui.selecting.className = "ui-state-default";
				}

				// Move on to the next element
				numSelecting++;
			},
	
			stop: function()
			{
				// Reset selectedItems and numSelecting to 0
				selectedItems.length = 0;
				numSelecting = 0;
				var selectAll = false;
				var removeAll = false;
		
				// For every selected element, add it to the array
				$(".ui-selected", this).each(function() 
				{
				    selectedItems.push(this.innerHTML);
				    
				});
				updateSelectedWeeks(selectedItems);
				$("#Weeks").val(selectedItems.toString());
			},
	
			distance: 1			// This is so we can register normal mouse click events
		});

		// Since the distance on the selectable is now greater than zero, clicks do not work if the mouse does not move
		// Simulate mouse click like the selector would normally
		$("#weekSelector li").click(function()
		{
			var clickedText = $(this).text();
	
			// If element is not selected, add it to array and add selected class
			if (selectedItems.indexOf(clickedText) == -1)
			{						
				$(this).addClass('ui-selected');
			}
			else 	// If element is already selected, then remove it from the array and remove selected class
			{
				$(this).removeClass('ui-selected');
			}
	
			// Reset selected items
			selectedItems.length = 0;
	
			// For every selected element, add it to the array
			$(".ui-selected").each(function() 
			{
				selectedItems.push($(this).text());
			});				
	
			updateSelectedWeeks(selectedItems);
			$("#Weeks").val(selectedItems.toString());
		});
	});

function updateSelectedWeeks(selectedItems)
{		
	var length = selectedItems.length;
	var output = [];
	var i, j;
	
	for (i = 0; i < length; i = j + 1)
	{
	    // Beginning of range or single
	    output.push(selectedItems[i]);
		
	    // Find end of range
	    for (var j = i + 1; j < length && parseInt(selectedItems[j]) == parseInt(selectedItems[j-1]) + 1; j++);
	    j--;
		
	    if (i == j) 
	    {
	        // single number
	        output.push(",");
	    } 
	    else 
	    {
	        if (i == j)
	        {
	            // two numbers
	            output.push(",", selectedItems[j], ",");
	        }
	        else 
	        { 
	            // range
	            output.push("-", selectedItems[j], ",");
	        }		
	    } 		
	}
	output.pop(); // remove trailing comma
	return output.join("");
}
function setSelectedWeeks(weeks)
{
    selectedItems = weeks.split(",");
    $("#weekSelector").children().each(function () {
        for (var k = 0; k < selectedItems.length; k++) {
            if (this.innerHTML == selectedItems[k]) {
                $(this).addClass('ui-selected');
                break;				// Exit current loop cycle once match is found
            }
            else {
                $(this).removeClass('ui-selected');
            }
        }
    });
}
