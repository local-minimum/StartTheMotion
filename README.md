# Thoughts

1. OnBezierEnd is caused by point.
2. Something on curve object captures this

1. Something asks game point to change to a curve and position
2. Game point checks if it may swap from curve A to B at position and does so
 via registered callback I suppose
