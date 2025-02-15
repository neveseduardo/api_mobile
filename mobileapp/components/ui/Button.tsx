import React from 'react';
import { TouchableOpacity, TouchableOpacityProps, TextStyle, Text } from 'react-native';
import clsx from 'clsx';

interface ButtonProps extends TouchableOpacityProps {
	title?: string;
	onPress: () => void;
	textStyle?: TextStyle;
	color?: 'primary' | 'info' | 'success' | 'warning' | 'danger' | 'default';
	circular?: boolean;
	children?: React.ReactNode;
	className?: string;
}

const Button = ({
	title,
	onPress,
	color = 'default',
	circular = false,
	style,
	textStyle,
	children,
	className,
	...rest
}: ButtonProps) => {
	const colorClasses = {
		primary: 'bg-primary-500',
		info: 'bg-blue-500',
		success: 'bg-green-500',
		warning: 'bg-yellow-500',
		danger: 'bg-red-500',
		default: 'bg-gray-800',
	}[color];

	const buttonClasses = clsx(
		colorClasses,
		circular ? 'rounded-full p-3' : 'rounded py-2 px-4',
		'flex justify-center items-center w-fit h-[40px]',
		className
	);

	return (
		<TouchableOpacity
			onPress={onPress}
			className={buttonClasses}
			style={style}
			{...rest}
		>
			{children ? (
				children
			) : (
				<Text className={clsx('text-white', textStyle)}>{title}</Text>
			)}
		</TouchableOpacity>
	);
};

export default Button;