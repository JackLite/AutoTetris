//
//  ByteBrewiOSHandler.h
//  ByteBrewiOSPlugin
//
//  Created by Cameron Hozouri on 3/19/20.
//  Copyright © 2020 Mad Cow Studios Inc. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ByteBrewHandler : NSObject
+(void) SendBackMessage:(char *) status;
+(void) RemoteConfigsHaveBeenUpdated;
+(void) LowLevelPushStart;
@end
